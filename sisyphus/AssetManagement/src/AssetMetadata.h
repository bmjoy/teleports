#pragma once
#include "uuid.h"
#include "json.hpp"
#include "Utils\String.h"
#include "Utils\UuidJsonSerializer.h"

namespace AssetManagement {
	class AssetMetadata;
}

namespace nlohmann {
	template<>
	struct adl_serializer<AssetManagement::AssetMetadata>;
}

namespace AssetManagement {
	class AssetMetadata {
	public:
		uuids::uuid GetId() const;
		String GetName() const;

	private:
		uuids::uuid id;
		String name;

		friend struct nlohmann::adl_serializer<AssetManagement::AssetMetadata>;
	};
}

namespace nlohmann {
	template<>
	struct adl_serializer<AssetManagement::AssetMetadata> {
		static void to_json(json& j, const AssetManagement::AssetMetadata& metadata) {
			j["id"] = metadata.GetId();
			j["name"] = metadata.GetName();
		}
		static void from_json(const json& j, AssetManagement::AssetMetadata& metadata) {
			metadata.id = j["id"];
			metadata.name = j["name"];
		}
	};
}