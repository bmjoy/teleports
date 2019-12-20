#pragma once
#include "ECS/Component.h"
#include "Vulkan.h"

namespace Sisyphus::Rendering::Vulkan {

	class PhysicalDevice : public ECS::IVulkanComponent<vk::PhysicalDevice> {
	public:
		~PhysicalDevice();

		void Initialize(const ECS::Entity& inComponentManager) override;
		static uuids::uuid TypeId();
		static std::string ClassName();
		static ECS::ComponentReferences Dependencies();

		vk::PhysicalDevice GetVulkanObject() const override;
		void HandleEvent(ECS::ComponentEvents::Initialization, const uuids::uuid& compTypeId) override;
		static ECS::ComponentReferences WatchList(ECS::ComponentEvents::Initialization);

		uint32_t GetGraphicsQueueFamilyIndex() const;
		uint32_t GetPresentQueueFamilyIndex() const;
		std::vector<vk::DeviceQueueCreateInfo> GetDeviceQueueCreateInfos() const;

		void Inspect() const;
	private:
		void FindGraphicsQueueFamilyIndex();
		void FindPresentQueueFamilyIndex(vk::SurfaceKHR surface);

		const ECS::Entity* entity;
		vk::PhysicalDevice physicalDevice;
		std::optional<uint32_t> graphicsQueueFamilyIndex;
		std::optional<uint32_t> presentQueueFamilyIndex;
	};
}