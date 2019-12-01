#include "Instance.h"
#include "Vulkan.h"
#include "VulkanUtils.h"
#include "DebugMessenger.h"
#include "platform_specific/PlatformSpecific.h"

namespace Sisyphus::Rendering::Vulkan {
	constexpr bool enableValidationLayers =
#if _DEBUG
	true;
#else
	false;
#endif

	std::vector<const char*> Instance::GetLayerNames()
	{
		std::vector<const char*> result;
		if constexpr (!enableValidationLayers) {
			return result;
		}

		result.push_back("VK_LAYER_LUNARG_standard_validation");

		for (auto&& name : result) {
			if (!IsLayerEnabled(name)) {
				SIS_THROW(std::string("Cannot find layer ") + std::string(name));
			}
		}

		return result;
	}

	Instance::Instance() :
		Component(),
		instance(nullptr),
		debugMessenger(nullptr)
	{
	}

	Instance::~Instance() = default;

	void Instance::OnInitialize()
	{
		vk::ApplicationInfo applicationInfo(
			"Vulkan App",
			VK_MAKE_VERSION(1, 0, 0),
			"Sisyphus",
			VK_MAKE_VERSION(1, 0, 0),
			VK_API_VERSION_1_0
		);

		std::vector<const char*> instanceExtensionNames = PlatformSpecific::GetInstanceExtensionNames();
		instanceExtensionNames.push_back(VK_KHR_SURFACE_EXTENSION_NAME);
		if constexpr (enableValidationLayers) {
			instanceExtensionNames.push_back(VK_EXT_DEBUG_UTILS_EXTENSION_NAME);
		}

		std::vector<const char*> layerNames = GetLayerNames();

		vk::InstanceCreateInfo instanceCreateInfo(
			{},
			&applicationInfo,
			static_cast<uint32_t>(layerNames.size()),
			layerNames.data(),
			static_cast<uint32_t>(instanceExtensionNames.size()),
			instanceExtensionNames.data()
		);

		instance = vk::createInstanceUnique(instanceCreateInfo);

		if constexpr (enableValidationLayers) {
			debugMessenger = std::make_unique<DebugMessenger>(*instance);
		}
	}
	ComponentType Instance::GetType() const
	{
		return ComponentType::Instance;
	}
	std::vector<ComponentType> Instance::GetDependencies() const
	{
		return std::vector<ComponentType>();
	}
	vk::Instance Instance::GetVulkanObject() const
	{
		return *instance;
	}
	Instance::operator vk::Instance() const
	{
		return GetVulkanObject();
	}
}
